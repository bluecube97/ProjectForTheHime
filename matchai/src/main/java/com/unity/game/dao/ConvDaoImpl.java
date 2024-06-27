package com.unity.game.dao;

import org.springframework.stereotype.Repository;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.logging.Logger;

@Repository
public class ConvDaoImpl implements ConvDao {

    @Override
    public String getConv(String scriptPath, String userConv, String jsonPath) {
        ProcessBuilder processBuilder = new ProcessBuilder("python", scriptPath, userConv);
        String result = "";

        try {
            Process process = processBuilder.start();
            BufferedReader reader = new BufferedReader(new InputStreamReader(process.getInputStream()));
            result = reader.readLine();
        } catch (IOException e) {
            Logger.getGlobal().severe(e.getMessage());
        }

        System.out.println("result: " + result);
        return result;
    }
}
