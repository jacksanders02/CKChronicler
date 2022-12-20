import re

from bs4 import BeautifulSoup
import requests
import json


# Takes a list of effects in HTML, and parses them to a dictionary
def parse_effects(effects):
    effects_data = []
    effects_list = effects.find_all('li')

    # If there is only one effect, there is no list, so create one from the
    # passed HTML
    if len(effects_list) == 0:
        effects_list = [effects]

    for e in effects_list:
        current_effect = { }
        current_effect["effected_attr"] = ''.join(re.findall(r'[^0-9.\-−+%]',
                                                             e.text)).strip()

        mag = e.find_all('b')
        pos = e.find_all('span')

        current_effect["effect_magnitude"] = mag[0].text.strip().replace("−","-") if mag else ""
        current_effect["positivity"] = pos[0]["class"][0].strip() if pos else ""

        effects_data.append(current_effect)
    return effects_data


# Parses a 'regular' table structure to a list of dictionaries (one per trait)
def regular_table(traits_table):
    # Remove unwanted subtables (i.e when a trait has different names under
    # different conditions)
    to_clear = traits_table.find_all("table")
    for tc in to_clear:
        tc.extract()

    # Get headings
    heading_data = []

    headings = traits_table.find_next('tr').find_all('th')
    for h in headings:
        if h.has_attr("colspan"):
            heading_data.extend(
                [f"{h.text.strip()}_{i}" for i in range(int(h["colspan"]))])
        else:
            heading_data.append(h.text.strip())

    # Find important indices
    icon_index = heading_data.index("Trait_0")
    name_index = heading_data.index("Trait_1")
    effects_index = [i for i, w in enumerate(heading_data) if "effect" in
                     w.lower()][0]

    description_index = heading_data.index("Description")

    traits = []
    rows = traits_table.find_all('tr')[1:]
    for row in rows:
        cols = row.find_all('td')

        if len(cols) == 0:
            print(row.prettify())

        row_data = []
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

        traits.append({
            "name": row_data[1],
            "icon": row_data[0],
            "effects": row_data[2],
            "description": row_data[3]
        })
    return traits


# Website I'm scraping the trait data from
TRAITS_PAGE = requests.get("https://ck3.paradoxwikis.com/Traits")

soup = BeautifulSoup(TRAITS_PAGE.content, "html.parser")

# 'Normal' table names
NORMALS = ["Congenital_traits", "Physical_traits", "Lifestyle_traits",
           "Commander_traits", "Infamous_traits", "Coping_mechanisms",
           "Dynasty_traits", "Decision_traits", "Event_traits",
           "Unobtainable_traits"]

traits = []
for t_name in NORMALS:
    print(t_name)
    table_title = soup.find(id=t_name)
    table = table_title.find_next("tbody")
    traits.extend(regular_table(table))

print(json.dumps(traits, indent=4))