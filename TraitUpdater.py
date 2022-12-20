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

HEADING_EFFECTS = ["Childhood_traits", "Health_traits", "Diseases"]

LEVELED_CONGEN = SOUP.find(id="Leveled_congenital_traits").find_next("tbody")

PERSONALITY = SOUP.find(id="Personality_traits").find_next("tbody")

LIFE = SOUP.find(id="Leveled_Activities/Lifestyle_traits").find_next("tbody")
EDUCATION = SOUP.find(id="Education_traits").find_next("tbody")

DESCENDANTS = SOUP.find(id="Descendant_traits").find_next("tbody")


# Takes a list of effects in HTML, and parses them to a dictionary
def parse_effects(effects):
    effects_data = []
    effects_list = effects.find_all('li')

    # If there is only one effect, there is no list, so create one from the
    # passed HTML
    if len(effects_list) == 0:
        effects_list = [effects]

    # Clean up effects
    for e in effects_list:
        # Remove superscript citations
        if sups := e.find_all("sup"):
            for s in sups:
                s.extract()

        # Fix up instances of multiple effects in one list item
        if p_tag := e.find_all("p"):
            effects_list = [p_tag[0]]
            p_tag[0].extract()
            effects_list.append(e)

    for e in effects_list:
        if "mutually exclusive" in e.text.lower():
            continue

        attr = ''.join(re.findall(r'[^0-9.\-−+%]', e.text)).strip()
        mag = ''.join(re.findall(r'[0-9.\-−+%]', e.text)).strip()
        pos = e.find_all('span')

        current_effect = {
            # Remove hidden: prefix and capitalise first  letter
            "effected_attr": attr.removeprefix("Hidden:").strip().capitalize(),
            # Replace heavy minus with normal dash, for compatibility
            "effect_magnitude": mag.replace("−", "-"),
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
                icon_name = img['src'].rsplit("/", 1)[0].replace("/thumb", "")
                row_data.append("https://ck3.paradoxwikis.com" + icon_name)
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
    # Get description table, as descriptions are stored separately from the
    # trait data
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

            icon = desc[icon_index].find_next('img')['src']
            icon_name = icon.rsplit("/", 1)[0].replace("/thumb", "")

            table_traits.append({
                "name": desc[name_index].text.strip(),
                "icon": "https://ck3.paradoxwikis.com" + icon_name,
                "effects": parse_effects(trait[effects_index]),
                "description": desc[desc_index].text.strip()
            })
    return table_traits


# Handle unique leveled congenital traits table
def leveled_congenital(traits_table):
    # Skip table headings
    rows = traits_table.find_all("tr")[1:]
    table_traits = []

    for row in rows:
        # Skip trait group
        cols = row.find_all("td")[1:]
        for td in cols:
            other_name = td.find_all("div")
            icon = td.find_next("img")['src']
            icon_name = icon.rsplit("/", 1)[0].replace("/thumb", "")
            table_traits.append({
                "name": td["id"] + (f" / {other_name[0]['id']}" if other_name
                                    else ""),
                "icon": "https://ck3.paradoxwikis.com" + icon_name,
                "effects": parse_effects(td.find_next("ul"))
            })

    return table_traits


# Handle unique leveled lifestyle traits table
def leveled_lifestyle(traits_table):
    # Skip table headings
    rows = traits_table.find_all("tr")[1:]
    table_traits = []

    for row in rows:
        cols = row.find_all("td")
        # Skip trait group
        icon = cols[0].find_next("img")['src']
        icon_name = icon.rsplit("/", 1)[0].replace("/thumb", "")

        for td in cols[2:]:
            list_items = td.find_all("li")

            # Find and get nam, then extract from document
            name_item = list_items[0]
            name = name_item.find_next("b")
            name_item.extract()

            table_traits.append({
                "name": name.text.strip(),
                "icon": "https://ck3.paradoxwikis.com" + icon_name,
                "effects": parse_effects(td)
            })

    return table_traits


# Handle unique education traits table
def education_traits(traits_table):
    # Skip table headings
    rows = traits_table.find_all("tr")[1:]
    table_traits = []
    current_attr = None

    for row in rows:
        cols = row.find_all("td")
        if cols[0].has_attr("rowspan"):
            current_attr = cols[0].text.strip()
            cols = cols[1:]

        icon = cols[0].find_next("img")['src']
        icon_name = icon.rsplit("/", 1)[0].replace("/thumb", "")

        table_traits.append({
            "name": cols[0].text.strip(),
            "icon": "https://ck3.paradoxwikis.com" + icon_name,
            "effects": [],
            "description": cols[4].text.strip()
        })

        # Handles town maven branch of education, that adds to both
        # stewardship and learning (written as Stewardship-Learning on the wiki)
        for ca in current_attr.split("-"):
            table_traits[-1]["effects"].append({
                "effected_attr": ca,
                "effect_magnitude": cols[1].text.strip().replace("−", "-"),
                "positivity": "effect-green"
            })

        # Only add lifestyle experience if the trait actually has that effect
        if cols[2].text.strip():
            table_traits[-1]["effects"].append({
                "effected_attr": f"Monthly {current_attr} Lifestyle "
                                 f"Experience",
                "effect_magnitude": cols[2].text.strip().replace("−", "-"),
                "positivity": "effect-green"
            })

    return table_traits


# Handle trait tables where the effects are in the headings
def heading_effects(traits_table):
    useless_headings = [r"Trait_", r"^Curable$", r"^Congenital$", r"^Cost$",
                        r"^Weight$", r"AI", r"^Wrong education$",
                        r"^Description$"]

    # Create regex for checking all useless headings
    regex = "(" + ")|(".join(useless_headings) + ")"
    heading_data = get_heading_data(traits_table)

    rows = traits_table.find_all("tr")[1:]

    icon_index = heading_data.index("Trait_0")
    name_index = heading_data.index("Trait_1")

    description_index = None
    if "Description" in heading_data:
        description_index = heading_data.index("Description")

    table_traits = []

    for tr in rows:
        cols = tr.find_all("td")
        icon = cols[icon_index].find_next("img")['src']
        icon_name = icon.rsplit("/", 1)[0].replace("/thumb", "")

        current_trait = {
            "name": cols[name_index].text.strip(),
            "icon": "https://ck3.paradoxwikis.com" + icon_name,
            "effects": []
        }

        if description_index:
            current_trait["description"] = cols[description_index].text.strip()

        for i, h in enumerate(heading_data):
            if re.match(regex, h) or cols[i].text.strip() == "0":
                continue

            pos = cols[i].find_all("span")

            current_trait["effects"].append({
                "effected_attr": h,
                "effect_magnitude": cols[i].text.strip().replace("−", "-"),
                "positivity": pos[0]["class"][0].strip() if pos else ""
            })

        table_traits.append(current_trait)

    return table_traits


# Handles weirdly-formatted descendant traits table(s)
def descendant_traits(traits_table):
    heading_data = get_heading_data(traits_table)

    # Find important indices
    icon_index = [i for i, w in enumerate(heading_data) if "trait_0" in
                     w.lower()][0]
    name_index = [i for i, w in enumerate(heading_data) if "trait_1" in
                     w.lower()][0]
    effects_index = [i for i, w in enumerate(heading_data) if "effect" in
                     w.lower()][0]

    table_traits = []
    rows = traits_table.find_all('tr')[1:]

    # Need to zip properties and descriptions together as they are in
    # separate tables for some reason
    for tr in rows:
        cols = tr.find_all('td')
        trait_data = [cols[:3], cols[4:]]

        for trait in trait_data:
            # Handle instances like compassionate, where a trait has rowspan=2
            if len(trait) == 0:
                continue

            icon = trait[icon_index].find_next('img')['src']
            icon_name = icon.rsplit("/", 1)[0].replace("/thumb", "")

            table_traits.append({
                "name": trait[name_index].text.strip(),
                "icon": "https://ck3.paradoxwikis.com" + icon_name,
                "effects": parse_effects(trait[effects_index])
            })

    return table_traits


traits = []

for t_name in NORMALS:
    table_title = SOUP.find(id=t_name)
    table = table_title.find_next("tbody")
    traits.extend(regular_table(table))

for t_name in HEADING_EFFECTS:
    table_title = SOUP.find(id=t_name)
    table = table_title.find_next("tbody")
    traits.extend(heading_effects(table))

traits.extend(personality_traits(PERSONALITY))
traits.extend(leveled_congenital(LEVELED_CONGEN))
traits.extend(leveled_lifestyle(LIFE))
traits.extend(education_traits(EDUCATION))
traits.extend(descendant_traits(DESCENDANTS))

with open("traits.json", 'w') as f:
    json.dump(traits, f, indent=4)