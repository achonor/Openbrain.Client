
-----------------------------math-------------------------------
-- 最小数值和最大数值指定返回值的范围。
-- @function [parent=#math] clamp
function math.clamp(v, minValue, maxValue)  
    if v < minValue then
        return minValue
    end
    if( v > maxValue) then
        return maxValue
    end
    return v 
end
---------------------------math end-----------------------------