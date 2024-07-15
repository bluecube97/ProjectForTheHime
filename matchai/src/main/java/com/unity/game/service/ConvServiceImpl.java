package com.unity.game.service;

import com.unity.game.dao.ConvDao;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import javax.servlet.ServletContext;

@Service
public class ConvServiceImpl implements ConvService {

    @Autowired
    private ConvDao convDao;
    @Autowired
    private ServletContext servletContext;

    @Override
    public String getConv(String userConv) {
        String scriptPath = servletContext.getRealPath("/resource/python/game/connectionManager.py");
        String jsonPath = servletContext.getRealPath("/resource/json/game/conversation.json");
        return convDao.getConv(scriptPath, userConv, jsonPath);
    }

    @Override
    public String loadGame(String pid) {
        String scriptPath = servletContext.getRealPath("/resource/python/game/load.py");
        String dstatusjson = servletContext.getRealPath("/resource/json/game/conversation.json");
        String chatjson = servletContext.getRealPath("/resource/json/game/conversation.json");

        return convDao.loadGame(scriptPath, pid,dstatusjson,chatjson);

    }

    @Override
    public String saveGame(String pid) {
        String scriptPath = servletContext.getRealPath("/resource/python/game/save.py");
        return convDao.saveGame(scriptPath,pid);

    }
}
