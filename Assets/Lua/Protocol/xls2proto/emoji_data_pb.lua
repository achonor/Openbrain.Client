-- Generated By protoc-gen-lua Do not Edit
local protobuf = require "protobuf.protobuf"
module('xls2proto/emoji_data_pb')


local EMOJI_DATA = protobuf.Descriptor();
local EMOJI_DATA_ID_FIELD = protobuf.FieldDescriptor();
local EMOJI_DATA_ICON_FIELD = protobuf.FieldDescriptor();
local EMOJI_DATA_ARRAY = protobuf.Descriptor();
local EMOJI_DATA_ARRAY_ITEMS_FIELD = protobuf.FieldDescriptor();

EMOJI_DATA_ID_FIELD.name = "id"
EMOJI_DATA_ID_FIELD.full_name = ".emoji_data.id"
EMOJI_DATA_ID_FIELD.number = 1
EMOJI_DATA_ID_FIELD.index = 0
EMOJI_DATA_ID_FIELD.label = 1
EMOJI_DATA_ID_FIELD.has_default_value = false
EMOJI_DATA_ID_FIELD.default_value = 0
EMOJI_DATA_ID_FIELD.type = 13
EMOJI_DATA_ID_FIELD.cpp_type = 3

EMOJI_DATA_ICON_FIELD.name = "icon"
EMOJI_DATA_ICON_FIELD.full_name = ".emoji_data.icon"
EMOJI_DATA_ICON_FIELD.number = 2
EMOJI_DATA_ICON_FIELD.index = 1
EMOJI_DATA_ICON_FIELD.label = 1
EMOJI_DATA_ICON_FIELD.has_default_value = false
EMOJI_DATA_ICON_FIELD.default_value = ""
EMOJI_DATA_ICON_FIELD.type = 9
EMOJI_DATA_ICON_FIELD.cpp_type = 9

EMOJI_DATA.name = "emoji_data"
EMOJI_DATA.full_name = ".emoji_data"
EMOJI_DATA.nested_types = {}
EMOJI_DATA.enum_types = {}
EMOJI_DATA.fields = {EMOJI_DATA_ID_FIELD, EMOJI_DATA_ICON_FIELD}
EMOJI_DATA.is_extendable = false
EMOJI_DATA.extensions = {}
EMOJI_DATA_ARRAY_ITEMS_FIELD.name = "items"
EMOJI_DATA_ARRAY_ITEMS_FIELD.full_name = ".emoji_data_ARRAY.items"
EMOJI_DATA_ARRAY_ITEMS_FIELD.number = 1
EMOJI_DATA_ARRAY_ITEMS_FIELD.index = 0
EMOJI_DATA_ARRAY_ITEMS_FIELD.label = 3
EMOJI_DATA_ARRAY_ITEMS_FIELD.has_default_value = false
EMOJI_DATA_ARRAY_ITEMS_FIELD.default_value = {}
EMOJI_DATA_ARRAY_ITEMS_FIELD.message_type = EMOJI_DATA
EMOJI_DATA_ARRAY_ITEMS_FIELD.type = 11
EMOJI_DATA_ARRAY_ITEMS_FIELD.cpp_type = 10

EMOJI_DATA_ARRAY.name = "emoji_data_ARRAY"
EMOJI_DATA_ARRAY.full_name = ".emoji_data_ARRAY"
EMOJI_DATA_ARRAY.nested_types = {}
EMOJI_DATA_ARRAY.enum_types = {}
EMOJI_DATA_ARRAY.fields = {EMOJI_DATA_ARRAY_ITEMS_FIELD}
EMOJI_DATA_ARRAY.is_extendable = false
EMOJI_DATA_ARRAY.extensions = {}

emoji_data = protobuf.Message(EMOJI_DATA)
emoji_data_ARRAY = protobuf.Message(EMOJI_DATA_ARRAY)

