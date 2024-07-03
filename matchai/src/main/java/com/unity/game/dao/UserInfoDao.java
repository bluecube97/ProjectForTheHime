package com.unity.game.dao;

import java.util.Map;

public interface UserInfoDao {
    int searchUserInfo(String param);

    void insertUserInfo(Map<String, Object> map);

    Map<String, Object> getUserInfo(String param);
}
