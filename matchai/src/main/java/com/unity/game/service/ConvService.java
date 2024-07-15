package com.unity.game.service;

public interface ConvService {
    String getConv(String userConv);

    String loadGame(String pid);

    String saveGame(String pid);
}
