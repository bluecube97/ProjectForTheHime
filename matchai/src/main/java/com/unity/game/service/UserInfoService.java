package com.unity.game.service;

import java.util.Map;

public interface UserInfoService {
    int searchUserInfo(String email);

    void insertUserInfo(Map<String, Object> map);

    Map<String, Object> getUserInfo(String param);
}
