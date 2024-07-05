package com.matchai.board.service;


import java.io.BufferedReader;
import java.io.InputStreamReader;

import org.springframework.stereotype.Service;

@Service
public class gameRatingService {

    public void pyRunner() {
    	String pyPath = "src/main/resources/py/getBaseballRating.py";
    	try {
    		ProcessBuilder pb = new ProcessBuilder("python", pyPath);
    		Process p = pb.start();
    		BufferedReader br = new BufferedReader(new InputStreamReader(p.getInputStream(), "utf-8"));
    	}catch (Exception e) {
			e.printStackTrace();
		}
    }
}
