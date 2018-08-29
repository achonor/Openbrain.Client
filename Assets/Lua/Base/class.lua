
function clone(object)
    local lookup_table = {}
    local function _copy(object)
        if type(object) ~= "table" then
            return object
        elseif lookup_table[object] then
            return lookup_table[object]
        end
        local new_table = {}
        lookup_table[object] = new_table
        for key, value in pairs(object) do
            new_table[_copy(key)] = _copy(value)
        end
        return setmetatable(new_table, getmetatable(object))
    end
    return _copy(object)
end

--Create an class.
function class(className, superName, superModulePath) 
    if superName ~= nil then
        require(superModulePath)
    end
    if _G[className] ~= nil then
        if _G[className].className == nil or _G[className].isLuaClass == nil then
            Debugger.LogError("Class name has been in the global,Please change the class name")
        end
        return
    end
    _G[className] = {}
    _G[className].className = className
    _G[className].isLuaClass = true
    _G[className].InitLuaClass = function(luaTable, valueTable)
        for key, value in pairs(valueTable) do
            luaTable[key] = value
        end
    end
        
        
    if _G[superName] ~= nil then
        _G[className].super = _G[superName]
        setmetatable(_G[className], _G[superName])
        _G[superName].__index = _G[superName]    
    end  
    
    _G[className].New = function(o)
        local o = o or {}
        setmetatable(o, _G[className])
        _G[className].__index = _G[className]
        return o
    end
    return _G[className]
end