import json
import os
from typing import Any
import logging

from Exceptions import MissingConfigurationException

class ConfigReader():
    def __init__(self):
        self.configuration = None
        self.logger = logging.getLogger("ConfigReader")
        self.logger.setLevel(logging.INFO)
        self.read()
    
    def read(self, root: str = None, as_return: bool = False) -> dict[str, Any]:
        try:
            # Force the use of the correct appsettings.json in PythonScripts
            if root is None:
                root = os.path.dirname(__file__)
            
            appsettings = self.find_filepath("appsettings.json", root)
            self.logger.info(f"Loading configuration from: {appsettings}")

            with open(appsettings, 'r') as f:
                config = json.load(f)
                if as_return:
                    return config
                else:
                    self.configuration = config
                    return self.configuration
        
        except FileNotFoundError:
            raise MissingConfigurationException("Configuration file 'appsettings.json' is missing. Please ensure it exists in the correct directory.")
        except json.JSONDecodeError:
            raise MissingConfigurationException("Configuration file 'appsettings.json' is corrupt or contains invalid JSON.")

    def get_configuration(self, *path: str, config = None) -> str | Any:
        if config is None:
            config = self.configuration
        
        if config is None:
            raise MissingConfigurationException("No configuration found. Ensure 'appsettings.json' is loaded correctly.")
        
        result = config
        for section in path:
            if section in result:
                result = result[section]
            else:
                raise MissingConfigurationException(f"Missing configuration section: {'.'.join(path)}")
        
        return result

    def find_filepath(self, filename: str, root: str = None) -> str:
        if filename is None:
            raise ValueError("Filename cannot be None")
        
        current_dir = os.getcwd() if root is None else root
        if filename in os.listdir(current_dir):
            return os.path.join(current_dir, filename)
        
        parent_dir = os.path.dirname(current_dir)
        child_dirs = [os.path.join(current_dir, d) for d in os.listdir(current_dir) if self.__is_dir(d)]
        while [parent_dir] + child_dirs:
            if filename in os.listdir(parent_dir):
                return os.path.join(parent_dir, filename)
            else:
                parent_dir = os.path.dirname(parent_dir)

            for dir in child_dirs:
                if filename in os.listdir(dir):
                    return os.path.join(dir, filename)
            else:
                child_dirs = [os.path.join(child, d) for child in child_dirs for d in os.listdir(child) if self.__is_dir(d)]
                
        raise FileNotFoundError(filename)

    def get_solution_root(self, solution: str = None) -> str:
        if solution is None:
            solution = "OctoWhirl"
        
        solution = f"{solution}.sln"
        return self.find_filepath(solution)
            
    def get_project_location(self, project: str, root: str = None) -> str:
        if ".csproj" in project and not project.endswith(".csproj"):
            raise ValueError('Project name must end with ".csproj"')
        else:
            project = f"{project}.csproj"
        
        if root is None:
            root = os.path.dirname(self.get_solution_root())
        
        dirs = [os.path.join(root, d) for d in os.listdir(root) if self.__is_dir(os.path.join(root, d))]
        while dirs:
            new_dirs = []
            for dir in dirs:
                if project in os.listdir(dir):
                    return os.path.join(dir, project)
                else:
                    new_dirs += [os.path.join(dir, d) for d in os.listdir(dir) if self.__is_dir(os.path.join(dir, d))]
            dirs = new_dirs
        
        raise ValueError(f"Service {project} cannot be located from {root}")
    
    def __is_dir(self, path: str) -> bool:
        return (
            os.path.isdir(path) 
            and not os.path.basename(path) in ("bin", "obj", ".vs", ".git", ".idea")
        )