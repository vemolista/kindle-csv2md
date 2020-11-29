# %%
import pandas as pd
import numpy as np


# TODO invoke file explorer

file = pd.read_csv('export.csv', encoding='utf-8')

# remove metadata
file = file.iloc[7:]

file = file.to_numpy().tolist()
data = []

for i in range(0, len(file)):
    data.append({
        "Type": file[i][0],
        "Page": file[i][1],
        "Content": file[i][3]
    })

export = ""
for entry in data:
    export += "- {}, {}".format(entry["Page"], entry["Type"])
    export += "\n"
    export += "  - {}".format(entry["Content"])
    export += "\n"

print(export)

markdown_file = open("export.md", "w")
markdown_file.write(export)
markdown_file.close()
