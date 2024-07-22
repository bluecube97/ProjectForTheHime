package com.matchai.board.dao;

import com.matchai.board.mapper.GameMapper;
import org.springframework.beans.factory.annotation.Autowired;

import java.util.HashMap;
import java.util.List;

public interface GameDao {


    List<HashMap<String, Object>> getGameList();
}
