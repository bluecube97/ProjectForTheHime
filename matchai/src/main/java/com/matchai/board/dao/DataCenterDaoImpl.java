package com.matchai.board.dao;

import com.matchai.board.mapper.DataCenterMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Map;

@Repository
public class DataCenterDaoImpl implements DataCenterDao {

    @Autowired
    private DataCenterMapper dataCenterMapper;

    @Override
    public List<Map<String, Object>> getKboBatList() {
        return dataCenterMapper.getKboBatList();
    }

    @Override
    public List<Map<String, Object>> getKboPitList() {
        return dataCenterMapper.getKboPitList();
    }

    @Override
    public List<Map<String, Object>> getMlbBatList() {
        return dataCenterMapper.getMlbBatList();
    }

    @Override
    public List<Map<String, Object>> getMlbPitList() {
        return dataCenterMapper.getMlbPitList();
    }
}
