# WiiRegionExporter

This tool retrieves a list of Wii regions and exports them as the region_info.h file used in fishguy's AnyGlobe Changer: https://github.com/fishguy6564/AnyGlobe-Changer/blob/master/source/region_info.h

The Extended Regions' default sheet can be found here: https://docs.google.com/spreadsheets/d/1mSAomO_msfNllNsPeXbgU6UbJaGV5t6NvbZi6ebPFx4

To create your own, place a data folder in the executable's path and create a file named credentials.txt. In it, fill in the values:

```
clientId=(your Google API client ID)
clientSecret=(your Google API client secret)
spreadsheetId=(the spreadsheet's ID, for example, the Extended Regions' one is 1mSAomO_msfNllNsPeXbgU6UbJaGV5t6NvbZi6ebPFx4)
sheetName=(the sheet name, for example, Regions (v1.3 WIP)
```
