package com.matchai.board.dao;

import com.matchai.board.mapper.FreeBoardMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Map;

@Repository
public class FreeBoardDaoImpl implements FreeBoardDao {

    @Autowired
    private FreeBoardMapper freeboardmapper;

    @Override
    public List<Map<String, Object>> getFreeBoardList() {
        return freeboardmapper.getFreeBoardList();
    }
}
