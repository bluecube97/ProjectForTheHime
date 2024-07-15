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

    @Override
    public String loadGame(String scriptPath, String email, String dstatusjson, String chatjson) {
        System.out.println("loadGame Method to Python " + email);
        ProcessBuilder processBuilder = new ProcessBuilder("python", scriptPath, email);
        String result = "";

        try {
            Process process = processBuilder.start();
            BufferedReader reader = new BufferedReader(new InputStreamReader(process.getInputStream()));
            BufferedReader errorReader = new BufferedReader(new InputStreamReader(process.getErrorStream()));

            String line;
            StringBuilder output = new StringBuilder();
            while ((line = reader.readLine()) != null) {
                output.append(line);
            }

            StringBuilder errors = new StringBuilder();
            while ((line = errorReader.readLine()) != null) {
                errors.append(line);
            }

            result = output.toString();
            if (errors.length() > 0) {
                System.err.println("Python script errors: " + errors.toString());
            }

        } catch (IOException e) {
            Logger.getGlobal().severe(e.getMessage());
        }
        return result;
    }

    @Override
    public String saveGame(String scriptPath, String pid) {
        ProcessBuilder processBuilder = new ProcessBuilder("python", scriptPath, pid);
        String result = "";

        try {
            Process process = processBuilder.start();
            BufferedReader reader = new BufferedReader(new InputStreamReader(process.getInputStream()));
            BufferedReader errorReader = new BufferedReader(new InputStreamReader(process.getErrorStream()));

            String line;
            StringBuilder output = new StringBuilder();
            while ((line = reader.readLine()) != null) {
                output.append(line);
            }

            StringBuilder errors = new StringBuilder();
            while ((line = errorReader.readLine()) != null) {
                errors.append(line);
            }

            result = output.toString();
            System.out.println("Save Game : " + result);
            if (errors.length() > 0) {
                System.err.println("Python script errors: " + errors.toString());
            }

        } catch (IOException e) {
            Logger.getGlobal().severe(e.getMessage());
        }
        return result;
    }

}


