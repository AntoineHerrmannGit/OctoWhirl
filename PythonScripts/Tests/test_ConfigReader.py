import tempfile
import json
import os
import pytest
from Core.ConfigReader import ConfigReader
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


@pytest.fixture
def temp_config_setup():
    """Setup temporary configs for testing"""
    temp_dir = tempfile.TemporaryDirectory()
    config1_name = 'config1.json'
    config2_name = 'config2.json'
    config1 = {
        "SectionA": {"Value": 123},
        "Common": {"Name": "Alpha"}
    }
    config2 = {
        "SectionB": {"Value": 456},
        "Common": {"Name": "Beta"}
    }
    
    config1_path = temp_dir.name + '/' + config1_name
    config2_path = temp_dir.name + '/' + config2_name
    
    with open(config1_path, 'w') as f:
        json.dump(config1, f)
    with open(config2_path, 'w') as f:
        json.dump(config2, f)
    
    yield {
        'temp_dir': temp_dir,
        'config1_name': config1_name,
        'config2_name': config2_name,
        'config1': config1,
        'config2': config2
    }
    
    temp_dir.cleanup()


def test_stateless_config_usage(temp_config_setup):
    """Test stateless configuration usage"""
    setup = temp_config_setup
    
    config_a = ConfigReader.read(setup['config1_name'], root=setup['temp_dir'].name)
    config_b = ConfigReader.read(setup['config2_name'], root=setup['temp_dir'].name)
    
    # Test direct config access
    assert ConfigReader.get_configuration('SectionA', 'Value', config=config_a) == 123
    assert ConfigReader.get_configuration('Common', 'Name', config=config_a) == 'Alpha'
    assert ConfigReader.get_configuration('SectionB', 'Value', config=config_b) == 456
    assert ConfigReader.get_configuration('Common', 'Name', config=config_b) == 'Beta'
    
    # Test via dummy business class
    dummy = DummyBusiness(config_a, config_b)
    assert dummy.get_a_value() == 123
    assert dummy.get_b_value() == 456
    
    # Test missing section raises
    with pytest.raises(Exception):
        ConfigReader.get_configuration('SectionB', 'Value', config=config_a)
    with pytest.raises(Exception):
        ConfigReader.get_configuration('SectionA', 'Value', config=config_b)


def test_find_filepath_in_subfolder(temp_config_setup):
    """Test finding filepath in subfolder"""
    setup = temp_config_setup
    
    # Create a subfolder and a config file inside
    subfolder = setup['temp_dir'].name + '/subdir'
    os.makedirs(subfolder)
    sub_config_name = 'sub_config.json'
    sub_config_path = subfolder + '/' + sub_config_name
    
    with open(sub_config_path, 'w') as f:
        json.dump({"foo": "bar"}, f)
    
    # find_filepath should find the file from the root
    found_path = ConfigReader.find_filepath(sub_config_name, root=setup['temp_dir'].name)
    assert found_path.endswith(sub_config_name)


def test_find_filepath_in_parent(temp_config_setup):
    """Test finding filepath in parent directory"""
    setup = temp_config_setup
    
    # Create a config file in the temp_dir (parent)
    parent_config_name = 'parent_config.json'
    parent_config_path = setup['temp_dir'].name + '/' + parent_config_name
    
    with open(parent_config_path, 'w') as f:
        json.dump({"bar": "baz"}, f)
    
    # Now search from a subfolder
    subfolder = setup['temp_dir'].name + '/subdir2'
    os.makedirs(subfolder)
    found_path = ConfigReader.find_filepath(parent_config_name, root=subfolder)
    assert found_path.endswith(parent_config_name)


def test_read_missing_file_raises(temp_config_setup):
    """Test that reading missing file raises exception"""
    setup = temp_config_setup
    
    with pytest.raises(MissingConfigurationException):
        ConfigReader.read('notfound.json', root=setup['temp_dir'].name)


def test_find_filepath_invalid_root_directory():
    """Test with non-existent directory"""
    with pytest.raises(FileNotFoundError):
        ConfigReader.find_filepath('test.json', root='/path/that/does/not/exist')


def test_find_filepath_root_is_file_not_directory(temp_config_setup):
    """Test using file as root directory"""
    setup = temp_config_setup
    
    # Create a file and try to use it as root directory
    test_file = setup['temp_dir'].name + '/not_a_directory.txt'
    with open(test_file, 'w') as f:
        f.write("test")
    
    with pytest.raises(ValueError):
        ConfigReader.find_filepath('test.json', root=test_file)


def test_find_filepath_none_filename(temp_config_setup):
    """Test with None filename"""
    setup = temp_config_setup
    
    with pytest.raises(ValueError):
        ConfigReader.find_filepath(None, root=setup['temp_dir'].name)