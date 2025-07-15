import unittest
from Core.YFStock import YFStock

class TestYFStock(unittest.TestCase):
    def test_yfstock_cac(self):
        CAC_ticker = "CAC"
        stock = YFStock(CAC_ticker)
        stock.save_to_json()
        self.assertEqual(stock.ticker, CAC_ticker)
        self.assertEqual(stock.yf_ticker, "^FCHI")
        self.assertFalse(stock.data.history().empty)

if __name__ == "__main__":
    unittest.main()