import unittest
import tempfile
import json
from Core.ConfigReader import ConfigReader
import os
from Exceptions import MissingConfigurationException


class DummyBusiness:
    """Dummy class to simulate using two different configs in the same business object."""
    def __init__(self, config_a: dict, config_b: dict):
        self.config_a = config_a
        self.config_b = config_b

    def get_a_value(self):
        return ConfigReader.get_configuration('SectionA', 'Value', config=self.config_a)

    def get_b_value(self):
        return ConfigReader.get_configuration('SectionB', 'Value', config=self.config_b)

class TestConfigReaderStateless(unittest.TestCase):
    def setUp(self):
        self.temp_dir = tempfile.TemporaryDirectory()
        self.config1_name = 'config1.json'
        self.config2_name = 'config2.json'
        self.config1 = {
            "SectionA": {"Value": 123},
            "Common": {"Name": "Alpha"}
        }
        self.config2 = {
            "SectionB": {"Value": 456},
            "Common": {"Name": "Beta"}
        }
        config1_path = self.temp_dir.name + '/' + self.config1_name
        config2_path = self.temp_dir.name + '/' + self.config2_name
        with open(config1_path, 'w') as f:
            json.dump(self.config1, f)
        with open(config2_path, 'w') as f:
            json.dump(self.config2, f)

    def tearDown(self):
        self.temp_dir.cleanup()

    def test_stateless_config_usage(self):
        config_a = ConfigReader.read(self.config1_name, root=self.temp_dir.name)
        config_b = ConfigReader.read(self.config2_name, root=self.temp_dir.name)
        # Test direct config access
        self.assertEqual(ConfigReader.get_configuration('SectionA', 'Value', config=config_a), 123)
        self.assertEqual(ConfigReader.get_configuration('Common', 'Name', config=config_a), 'Alpha')
        self.assertEqual(ConfigReader.get_configuration('SectionB', 'Value', config=config_b), 456)
        self.assertEqual(ConfigReader.get_configuration('Common', 'Name', config=config_b), 'Beta')
        # Test via dummy business class
        dummy = DummyBusiness(config_a, config_b)
        self.assertEqual(dummy.get_a_value(), 123)
        self.assertEqual(dummy.get_b_value(), 456)
        # Test missing section raises
        with self.assertRaises(Exception):
            ConfigReader.get_configuration('SectionB', 'Value', config=config_a)
        with self.assertRaises(Exception):
            ConfigReader.get_configuration('SectionA', 'Value', config=config_b)

    def test_find_filepath_in_subfolder(self):
        # Create a subfolder and a config file inside
        import os
        subfolder = self.temp_dir.name + '/subdir'
        os.makedirs(subfolder)
        sub_config_name = 'sub_config.json'
        sub_config_path = subfolder + '/' + sub_config_name
        with open(sub_config_path, 'w') as f:
            json.dump({"foo": "bar"}, f)
        # find_filepath should find the file from the root
        found_path = ConfigReader.find_filepath(sub_config_name, root=self.temp_dir.name)
        self.assertTrue(found_path.endswith(sub_config_name))

    def test_find_filepath_in_parent(self):
        # Create a config file in the temp_dir (parent)
        parent_config_name = 'parent_config.json'
        parent_config_path = self.temp_dir.name + '/' + parent_config_name
        with open(parent_config_path, 'w') as f:
            json.dump({"bar": "baz"}, f)
        # Now search from a subfolder
        subfolder = self.temp_dir.name + '/subdir2'
        os.makedirs(subfolder)
        found_path = ConfigReader.find_filepath(parent_config_name, root=subfolder)
        self.assertTrue(found_path.endswith(parent_config_name))

    def test_read_missing_file_raises(self):
        with self.assertRaises(MissingConfigurationException):
            ConfigReader.read('notfound.json', root=self.temp_dir.name)

if __name__ == '__main__':
    unittest.main()