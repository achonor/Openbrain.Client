require("Base.class")

local LuaEmojiConfig = class("LuaEmojiConfig", "LuaDataReader", "Configs.LuaDataReader").New()


function LuaEmojiConfig:GetDataConfigName()
    return "emoji_data.bin"
end

function LuaEmojiConfig:GetProtoInstance()
    require("Protocol.xls2proto.emoji_data_pb")
    return xls2proto.emoji_data_pb.emoji_data_ARRAY()
end


function LuaEmojiConfig:OnLoad()
    self.super.OnLoad(self)
end

function LuaEmojiConfig:GetDataByID(id)
    return self.dataDict[id]
end


LuaEmojiConfig:OnLoad()

return LuaEmojiConfig