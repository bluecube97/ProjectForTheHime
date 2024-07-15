package com.unity.game.dao;

public interface ConvDao {
    String getConv(String scriptPath, String userConv, String jsonPath);

    String loadGame(String scriptPath, String pid, String dstatusjson, String chatjson);

    String saveGame(String scriptPath, String pid);
}
