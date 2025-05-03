from Models import Enum

class InvalidSourceException(Exception):
    def __init__(self, message=None):
        if message is None:
            message = "Invalid source"
        super().__init__(message)
        
    def __init__(self, enum: Enum, message=None):
        _message = f"Source must belong to {", ".join(enum)}. {message}"
        super().__init__(message)