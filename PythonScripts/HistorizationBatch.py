import sys
import os

# Add the PythonScripts directory to sys.path
sys.path.append(os.path.dirname(os.path.abspath(__file__)))

from Core import ConfigReader, YFStock
from logging import Logger


def main() -> None:
    logger = Logger(os.path.basename(__file__), level="INFO")
    
    logger.info("Loading config...")
    config = ConfigReader()
    if config is None:
        logger.error("Failed to load config.")
        return
    logger.info("Configuration Loaded !")
    
    tickers = config.get_configuration("Historization", "Tickers")
    logger.info(f"Starting archive for tickers {tickers}")
    for ticker in tickers:
        try:
            logger.info(f"Archiving ticker {ticker}")
            stock = YFStock(ticker)
            stock.save_to_json()
            logger.info(f"Stock {ticker} data saved.")
        except Exception as e:
            logger.error(f"Failed to save stock {ticker} data: {e}")
    
    logger.info("All stocks processed.")


if __name__ == "__main__":
    main()