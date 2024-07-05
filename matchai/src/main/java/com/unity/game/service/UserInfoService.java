package com.unity.game.service;

import java.util.HashMap;
import java.util.Map;

public interface UserInfoService {
    int searchUserInfo(String email);

    void insertUserInfo(Map<String, Object> map);

    Map<String, Object> getUserInfo(String param);

    void setDstate(HashMap<String, Object> dstats);

    void setChatLong(HashMap<String, Object> map);
}
