package com.unity.game.mapper;

import java.util.Map;

public interface UserInfoMapper {
    int searchUserInfo(String param);

    void insertUserInfo(Map<String, Object> map);

    Map<String, Object> getUserInfo(String param);
}
