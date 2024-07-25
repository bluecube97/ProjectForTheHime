package com.matchai.board.dao;

import java.util.List;
import java.util.Map;

public interface DataCenterDao {
    List<Map<String, Object>> getKboBatList();

    List<Map<String, Object>> getKboPitList();

    List<Map<String, Object>> getMlbBatList();

    List<Map<String, Object>> getMlbPitList();
}
