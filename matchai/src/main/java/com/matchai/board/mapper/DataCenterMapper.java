package com.matchai.board.mapper;

import org.apache.ibatis.annotations.Mapper;

import java.util.List;
import java.util.Map;

@Mapper
public interface DataCenterMapper {
    List<Map<String, Object>> getKboBatList();

    List<Map<String, Object>> getKboPitList();

    List<Map<String, Object>> getMlbBatList();

    List<Map<String, Object>> getMlbPitList();
}
