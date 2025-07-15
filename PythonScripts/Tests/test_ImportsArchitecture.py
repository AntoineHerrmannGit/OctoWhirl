import unittest

class TestImportsArchitecture(unittest.TestCase):
    """
    Tests to demonstrate that the new import architecture works
    from anywhere in the project.
    """
    
    def test_import_from_core_package(self):
        """Test that we can import directly from Core package"""
        try:
            from Core import ConfigReader, Stock, YFStock, MarketDataArchiver
            self.assertTrue(True, "Import from Core succeeded")
        except ImportError as e:
            self.fail(f"Failed to import from Core: {e}")
    
    def test_specific_absolute_imports(self):
        """Test specific absolute imports"""
        try:
            from Core.ConfigReader import ConfigReader
            from Core.Stock import Stock
            from Core.YFStock import YFStock
            from Core.MarketDataArchiver import MarketDataArchiver
            
            # Verify that classes are properly imported
            self.assertTrue(hasattr(ConfigReader, 'read'))
            self.assertTrue(hasattr(Stock, '__init__'))
            self.assertTrue(hasattr(YFStock, 'get_spots'))
            self.assertTrue(hasattr(MarketDataArchiver, '__init__'))
            
        except ImportError as e:
            self.fail(f"Failed absolute imports: {e}")
    
    def test_class_inheritance_works(self):
        """Test that inheritance works with new imports"""
        try:
            from Core.YFStock import YFStock
            from Core.Stock import Stock
            
            # We just test that classes are properly linked
            self.assertTrue(issubclass(YFStock, Stock))
            
        except Exception as e:
            self.fail(f"Problem with inheritance: {e}")
    
    def test_models_imports_still_work(self):
        """Test that Models imports still work"""
        try:
            from Models import Spot, CorporateAction
            self.assertTrue(True, "Models import succeeded")
        except ImportError as e:
            self.fail(f"Failed Models import: {e}")
    
    def test_exceptions_imports_still_work(self):
        """Test that Exceptions imports still work"""
        try:
            from Exceptions import MissingConfigurationException
            self.assertTrue(True, "Exceptions import succeeded")
        except ImportError as e:
            self.fail(f"Failed Exceptions import: {e}")

if __name__ == '__main__':
    unittest.main()
