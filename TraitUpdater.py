import re

from bs4 import BeautifulSoup
import requests
import json

# Website I'm scraping the trait data from
TRAITS_PAGE = requests.get("https://ck3.paradoxwikis.com/Traits")

SOUP = BeautifulSoup(TRAITS_PAGE.content, "html.parser")

# 'Normal' table names
NORMALS = ["Congenital_traits", "Physical_traits", "Lifestyle_traits",
           "Commander_traits", "Infamous_traits", "Coping_mechanisms",
           "Dynasty_traits", "Decision_traits", "Event_traits",
           "Unobtainable_traits"]

PERSONALITY = SOUP.find(id="Personality_traits").find_next("tbody")


# Takes a list of effects in HTML, and parses them to a dictionary
def parse_effects(effects):
    effects_data = []
    effects_list = effects.find_all('li')

    # If there is only one effect, there is no list, so create one from the
    # passed HTML
    if len(effects_list) == 0:
        effects_list = [effects]

    for e in effects_list:
        if "mutually exclusive" in e.text.lower():
            continue

        mag = e.find_all('b')
        pos = e.find_all('span')

        current_effect = {
            "effected_attr": ''.join(re.findall(r'[^0-9.\-−+%]',
                                                e.text)).strip(),
            "effect_magnitude": mag[0].text.strip().replace("−",
                                                            "-") if mag else "",
            "positivity": pos[0]["class"][0].strip() if pos else ""
        }

        effects_data.append(current_effect)
    return effects_data


def get_heading_data(table):
    # Get headings
    heading_data = []

    headings = table.find_next('tr').find_all('th')
    for h in headings:
        if h.has_attr("colspan"):
            heading_data.extend(
                [f"{h.text.strip()}_{i}" for i in range(int(h["colspan"]))])
        else:
            heading_data.append(h.text.strip())
    return heading_data


# Parses a 'regular' table structure to a list of dictionaries (one per trait)
def regular_table(traits_table):
    # Remove unwanted subtables (i.e when a trait has different names under
    # different conditions)
    to_clear = traits_table.find_all("table")
    for tc in to_clear:
        tc.extract()

    heading_data = get_heading_data(traits_table)

    # Find important indices
    icon_index = heading_data.index("Trait_0")
    name_index = heading_data.index("Trait_1")
    effects_index = [i for i, w in enumerate(heading_data) if "effect" in
                     w.lower()][0]

    description_index = heading_data.index("Description")

    table_traits = []
    rows = traits_table.find_all('tr')[1:]
    for row in rows:
        cols = row.find_all('td')

        row_data = []
        # Parse all necessary data
        for i in [icon_index, name_index, effects_index, description_index]:
            td = cols[i]
            if i == effects_index:
                row_data.append(parse_effects(td))
            elif td.text:
                row_data.append(td.text.strip())
            elif img := td.find_next('img'):
                row_data.append("https://ck3.paradoxwikis.com" + img['src'])
            else:
                row_data.append("")

        table_traits.append({
            "name": row_data[1],
            "icon": row_data[0],
            "effects": row_data[2],
            "description": row_data[3]
        })
    return table_traits


# Handles weirdly-formatted personality traits table(s)
def personality_traits(personality_table):
    description_table = personality_table.find_next('tbody')

    heading_data = get_heading_data(personality_table)
    description_headings = get_heading_data(description_table)

    # Find important indices
    icon_index = description_headings.index("Trait_0")
    name_index = description_headings.index("Trait_1")
    effects_index = [i for i, w in enumerate(heading_data) if "effect" in
                     w.lower()][0]
    desc_index = description_headings.index("Description")

    table_traits = []
    rows = personality_table.find_all('tr')[1:]
    description_rows = description_table.find_all('tr')[1:]

    # Need to zip properties and descriptions together as they are in
    # separate tables for some reason
    for (p_row, d_row) in zip(rows, description_rows):
        p_cols = p_row.find_all('td')
        d_cols = d_row.find_all('td')
        trait_data = [p_cols[:len(heading_data) // 2], p_cols[len(heading_data) // 2:]]
        descriptions = [d_cols[:len(description_headings) // 2], d_cols[len(description_headings) // 2:]]

        for trait, desc in zip(trait_data, descriptions):
            # Handle instances like compassionate, where a trait has rowspan=2
            if len(trait) == 0:
                continue

            icon = desc[icon_index].find_next('img')

            table_traits.append({
                "name": desc[name_index].text.strip(),
                "icon": "https://ck3.paradoxwikis.com" + icon['src'],
                "effects": parse_effects(trait[effects_index]),
                "description": desc[desc_index].text.strip()
            })
    return table_traits


traits = []

traits.extend(personality_traits(PERSONALITY))
for t_name in NORMALS:
    table_title = SOUP.find(id=t_name)
    table = table_title.find_next("tbody")
    traits.extend(regular_table(table))

print(json.dumps(traits, indent=4))