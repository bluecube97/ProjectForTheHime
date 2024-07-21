package com.matchai.board.service;

import com.matchai.board.dao.FreeBoardDao;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Map;

@Service
public class FreeBoardServiceImpl implements FreeBoardService {

    @Autowired
    private FreeBoardDao freeboarddao;

    @Override
    public List<Map<String, Object>> getFreeBoardList() {
        return freeboarddao.getFreeBoardList();
    }
}
