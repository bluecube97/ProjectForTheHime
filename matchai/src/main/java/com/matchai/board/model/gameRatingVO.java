package com.matchai.board.model;

import java.util.List;
import java.util.Map;

import org.springframework.stereotype.Component;

@Component
public class gameRatingVO {

	private List<Map<String, Object>> gameRating;

	public List<Map<String, Object>> getGameRating() {
		return gameRating;
	}

	public void setGameRating(List<Map<String, Object>> gameRating) {
		this.gameRating = gameRating;
	}
}
