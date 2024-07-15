package com.unity.game.dao;

import java.util.HashMap;
import java.util.Map;

public interface UserInfoDao {
    int searchUserInfo(String param);

    void insertUserInfo(Map<String, Object> map);

    Map<String, Object> getUserInfo(String param);

    void setDstate(HashMap<String, Object> dstats);

}
