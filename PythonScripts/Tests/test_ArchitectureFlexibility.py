import unittest

class TestArchitectureFlexibility(unittest.TestCase):
    """
    Tests to demonstrate the flexibility of the new architecture
    """
    
    def test_multiple_import_methods_work(self):
        """
        Demonstrates that the new architecture is more flexible.
        We can import from different places.
        """
        import_methods = []
        
        # Method 1: Direct import from Core
        try:
            from Core.ConfigReader import ConfigReader as CR1
            import_methods.append("Direct from Core")
        except ImportError:
            pass
        
        # Method 2: Import via Core package
        try:
            from Core import ConfigReader as CR2
            import_methods.append("Via Core package")
        except ImportError:
            pass
        
        # At least 2 methods should work
        self.assertGreaterEqual(len(import_methods), 2, 
                               f"Only {len(import_methods)} import methods work: {import_methods}")
    
    def test_api_clarity_with_all_exports(self):
        """Test that __all__ clearly defines the public API"""
        import Core
        
        # Verify that __all__ exists and contains our main classes
        self.assertTrue(hasattr(Core, '__all__'))
        expected_exports = ['ConfigReader', 'MarketDataArchiver', 'Stock', 'YFStock']
        
        for export in expected_exports:
            self.assertIn(export, Core.__all__, f"{export} missing from __all__")

if __name__ == '__main__':
    unittest.main()
