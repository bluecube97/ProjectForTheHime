package com.unity.game.dao;

import com.unity.game.mapper.OutingMapper;
import com.unity.game.mapper.UserInfoMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Repository;

import java.util.HashMap;
import java.util.Map;

@Repository
public class UserInfoDaoImpl implements UserInfoDao {

    @Autowired
    private UserInfoMapper userInfoMapper;


    @Override
    public int searchUserInfo(String param) {
        return userInfoMapper.searchUserInfo(param);
    }

    @Override
    public void insertUserInfo(Map<String, Object> map) {
        userInfoMapper.insertUserInfo(map);
    }

    @Override
    public Map<String, Object> getUserInfo(String param) {
        return userInfoMapper.getUserInfo(param);
    }

    @Override
    public void setDstate(HashMap<String, Object> dstats) {
        userInfoMapper.setDstate(dstats);

    }
}
