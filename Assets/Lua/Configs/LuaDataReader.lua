require("Base.class")

local LuaDataReader = class("LuaDataReader")


function LuaDataReader:GetDataConfigName()
    return nil
end

function LuaDataReader:GetProtoInstance()
    return nil
end

function LuaDataReader:OnLoad()
    self.dataDict = {}
    local configName = self:GetDataConfigName()
    if (nil == configName) then
        print("Need Override GetDataConfigName")
        return
    end
    local protoInstance = self:GetProtoInstance()
    if (nil == protoInstance) then
        print("Need Override GetProtoInstance")
        return
    end

    local filePath = nil
    if (GameConst.UsePersistent) then
        filePath = GameConst.persistentPath.."/DataConfig/"..configName
    else
        filePath = GameConst.streamingPath.."/DataConfig/"..configName
    end
    --读文件数据
    local file = assert(io.open(filePath, 'rb'))
    local fileData = file:read("*a")
    file:close()
    --反序列化
    protoInstance:ParseFromString(fileData)
    --数据处理
    for key, value in ipairs(protoInstance.items) do
        self.dataDict[value.id] = value
    end
    print("LuaDataReader:OnLoad "..configName.." end!")
    print(tostring(self.dataDict))
end

return LuaPlayConfig