import csv
import argparse
from retriever import insert_post

CHARACTERS = ["Karen", "Jade", "Kaden", "Aster"]

def populate_specific(csv_path):
    with open(csv_path, newline='', encoding='utf-8') as csvfile:
        reader = csv.DictReader(csvfile)
        for row in reader:
            character = row["Character"].strip()
            title = row["Title"].strip()
            text = row["Text"].strip()
            insert_post(character, title, text)

def populate_all(csv_path):
    with open(csv_path, newline='', encoding='utf-8') as csvfile:
        reader = csv.DictReader(csvfile)
        for row in reader:
            title = row["Title"].strip()
            text = row["Text"].strip()
            insert_post("common", title, text)

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Populate ChromaDB from CSV")
    parser.add_argument("csv_path", help="Path to the CSV file")
    parser.add_argument(
        "--mode",
        choices=["specific", "all"],
        required=True,
        help="'specific' uses per-row character, 'all' inserts each row into all collections"
    )

    args = parser.parse_args()

    if args.mode == "specific":
        populate_specific(args.csv_path)
    elif args.mode == "all":
        populate_all(args.csv_path)
