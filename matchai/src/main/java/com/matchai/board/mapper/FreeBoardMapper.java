package com.matchai.board.mapper;

import org.apache.ibatis.annotations.Mapper;

import java.util.List;
import java.util.Map;

@Mapper
public interface FreeBoardMapper {
    List<Map<String, Object>> getFreeBoardList();
}
