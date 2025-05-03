class EnumBase(type):
    def __iter__(cls):
        for name, value in vars(cls).items():
            if not name.startswith("_") and not name.startswith("__") and not callable(value):
                yield value
                
class Enum(metaclass=EnumBase):
    pass