# test_retriever.py

from retriever import retriever

SAMPLE_QUERY = "Karen is being rude at a store"

def test_retriever(character):
    print(f"\n===== Testing retriever for: {character} =====")
    result = retriever(character, SAMPLE_QUERY)
    assert isinstance(result, str), f"Result for {character} is not a string."
    assert len(result.strip()) > 0, f"No text returned for {character}."
    print(f"✓ Passed: Non-empty result for {character}")

if __name__ == "__main__":
    # CHARACTERS = ["Karen", "Jade", "Kaden", "Aster"]
    # for char in CHARACTERS:
    #     test_retriever(char)

    test_retriever("Karen")

    print("\nAll tests completed successfully.")
