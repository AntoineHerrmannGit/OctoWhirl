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
    
    def read(self, filename, root: str = None) -> dict[str, Any]:
        try:
            if root is None:
                root = os.path.dirname(__file__)
            
            appsettings = self.find_filepath(filename, root)
            self.logger.info(f"Loading configuration from: {appsettings}")

            with open(appsettings, 'r') as f:
                config = json.load(f)
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
        """
        Search for a file by name, recursively in the current directory and all parents/subfolders.
        Returns the absolute path if found, else raises ValueError.
        """
        if filename is None:
            raise ValueError("Filename cannot be None")
        
        current_dir = os.getcwd() if root is None else root

        for dirpath, dirnames, filenames in os.walk(current_dir):
            if filename in filenames:
                return os.path.join(dirpath, filename)
        
        parent_dir = os.path.dirname(current_dir)
        while parent_dir and parent_dir != current_dir:
            for dirpath, dirnames, filenames in os.walk(parent_dir):
                if filename in filenames:
                    return os.path.join(dirpath, filename)
            current_dir, parent_dir = parent_dir, os.path.dirname(parent_dir)

        raise FileNotFoundError(f"{filename} not found in any parent or child directories.")

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
    
    