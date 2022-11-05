# VAVACAR MVP App
## Setup files
### games.csv
Each line must contain name of game
### positions.csv
Each line must contain 

- name of game which must be same text as on games.csv
- name of position 
- abbreviation name

Each value should be separated by “;”
### ratings.csv
Each line must contain 

- name of game which must be same text as on games.csv
- name of rating
- name of position which must be same text as on positions.csv
- integer value of rating
- boolean value. If a rating is for initial rating, it should be “true”. If not it should be “false”.
- boolean value. If a rating is using for calculation of team score, it should be “true”. If not it should be “false”.

Each value should be separated by “;”
## Match files
First line must contain name of game which must be same text as on games.csv.

Other lines must contain 

- name of player
- nick name of player
- player number
- name of team
- abbreviation of position name which must be same text as on positions.csv
- integer value of ratings which should be in same order as on ratings.csv

Each value should be separated by “;”
## Using of App
You can just debug solution on Visual Studio.  When the app is ready, please type folder path which containes match files.