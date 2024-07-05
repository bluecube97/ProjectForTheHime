package com.unity.game.service;

import com.unity.game.dao.OutingDao;
import com.unity.game.dao.UserInfoDao;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.HashMap;
import java.util.Map;

@Service
public class UserInfoServiceImpl implements UserInfoService {

    @Autowired
    private UserInfoDao userInfoDao;

    @Override
    public int searchUserInfo(String param) {
        return userInfoDao.searchUserInfo(param);
    }

    @Override
    public void insertUserInfo(Map<String, Object> map) {
        userInfoDao.insertUserInfo(map);
    }

    @Override
    public Map<String, Object> getUserInfo(String param) {
        return userInfoDao.getUserInfo(param);
    }

    @Override
    public void setDstate(HashMap<String, Object> dstats) {
        userInfoDao.setDstate(dstats);
    }

    @Override
    public void setChatLong(HashMap<String, Object> map) {
        userInfoDao.setChatLong(map);

    }
}
