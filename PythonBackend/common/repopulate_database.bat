@echo off
echo Resetting the database...

REM Clear the database collections
python -c "from retriever import clear_all_collections; clear_all_collections()"

echo Database reset complete.
echo Populating the database...

REM Populate the database with the specific CSV file (change the path if needed)
python populate_chroma_from_csv.py "bobastop_populate - character_specific.csv" --mode specific

REM Populate the database with the common knowledge CSV file (change the path if needed)
python populate_chroma_from_csv.py "bobastop_populate - common_knowledge.csv" --mode all

echo Database population complete.
pause
