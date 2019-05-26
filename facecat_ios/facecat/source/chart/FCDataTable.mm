#include "stdafx.h"
#include "FCDataTable.h"

namespace FaceCat{
    /////////////////////////////////////////////////////////////////////////////////////
    FCDataRow::FCDataRow(FCDataTable *table){
        m_values.m_mode = 1;
        m_table = table;
    }
    
    FCDataRow::FCDataRow(FCDataTable *table, int capacity, int step){
        m_values.m_mode = 1;
        m_table = table;
        m_values.setCapacity(capacity);
        m_values.setStep(step);
    }
    
    FCDataRow::FCDataRow(FCDataTable *table, double *ary, int size){
        m_values.m_mode = 1;
        m_table = table;
        m_values.addranges(ary, size);
    }
    
    FCDataRow::~FCDataRow(){
        m_values.clear();
        m_values = 0;
        m_table = 0;
    }
    
    void FCDataRow::fillEmpty(int columns){
        int size = m_values.size();
        if(size != -1){
            for(int i = size; i < columns; i++){
                m_values.insert(i, m_table->NaN);
            }
        }
    }
    
    double FCDataRow::get(int index){
        if(index >= 0 && index < m_values.size()){
            return m_values.get(index);
        }
        return m_table->NaN;
    }
    
    void FCDataRow::remove(int index){
        if(index != -1){
            m_values.removeAt(index);
        }
    }
    
    void FCDataRow::set(int index, double value){
        if(index >= 0 && index < m_values.size()){
            m_values.set(index, value);
        }
    }
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    FCDataTable::FCDataTable(){
        m_keys.m_mode = 1;
        m_columns.m_mode = 1;
        m_rows.m_mode = 1;
        unsigned int nan[2] ={0xffffffff, 0x7fffffff};
        NaN = *(double*)nan;
        m_colsCapacity = 4;
        m_colsStep = 4;
        m_autoField = 10000;
    }
    
    FCDataTable::~FCDataTable(){
        clear();
        for(int i = 0; i < m_columns.size(); i++){
            delete[] m_columns.get(i);
        }
        m_columns.clear();
    }
    
    FCDataRow* FCDataTable::addKey(double num){
        if(m_keys.size() == 0 || num > m_keys.get((int)m_keys.size() - 1)){
            m_keys.add(num);
            FCDataRow *row = new FCDataRow(this, m_colsCapacity, m_colsStep);
            m_rows.add(row);
            return row;
        }
        else{
            int begin = 0;
            int end = m_rows.size() - 1;
            int sub = end - begin;
            while(sub > 1){
                int half = begin + sub / 2;
                double hf = m_keys.get(half);
                if(hf > num){
                    end = half;
                }
                else if(hf < num){
                    begin = half;
                }
                sub = end - begin;
            }
            if(num < m_keys.get(begin)){
                m_keys.insert(begin, num);
                FCDataRow *row = new FCDataRow(this, m_colsCapacity, m_colsStep);
                m_rows.insert(begin, row);
                return row;
            }
            else if(num > m_keys.get(end)){
                m_keys.insert(end + 1, num);
                FCDataRow *row = new FCDataRow(this, m_colsCapacity, m_colsStep);
                m_rows.insert(end + 1, row);
                return row;
            }
            else{
                m_keys.insert(begin + 1, num);
                FCDataRow *row = new FCDataRow(this, m_colsCapacity, m_colsStep);
                m_rows.insert(begin + 1, row);
                return row;
            }
        }
    }
    
    void FCDataTable::fillEmpty(){
        int colSize = m_columns.size();
        for (int i = 0; i < m_rows.size(); i++){
            m_rows.get(i)->fillEmpty(colSize);
        }
    }
    
    int FCDataTable::AUTOFIELD(){
        return m_autoField++;
    }
    
    void FCDataTable::addColumn(int colName){
        int colSize = m_columns.size();
        int *column = new int[2];
        column[0] = colName;
        column[1] = colSize;
        if (colSize == 0){
            m_columns.add(column);
        }
        else{
            int begin = 0;
            int end = colSize - 1;
            int sub = end - begin;
            while (sub > 1){
                int half = begin + sub / 2;
                int hf = m_columns.get(half)[0];
                if (hf > colName){
                    end = half;
                }
                else if (hf < colName){
                    begin = half;
                }
                sub = end - begin;
            }
            if (colName < m_columns.get(begin)[0]){
                m_columns.insert(begin, column);
                fillEmpty();
                return;
            }
            else if (colName > m_columns.get(end)[0]){
                m_columns.insert(end + 1, column);
                fillEmpty();
                return;
            }
            else{
                m_columns.insert(begin + 1, column);
                fillEmpty();
                return;
            }
        }
    }
    
    void FCDataTable::addRow(double pk, double *ary, int size){
        m_keys.add(pk);
        FCDataRow *row = new FCDataRow(this, ary, size);
        m_rows.add(row);
    }
    
    void FCDataTable::clear(){
        m_keys.clear();
        for(int i = 0;i < m_rows.size();i++){
            FCDataRow *row = m_rows.get(i);
            if(row){
                delete row;
            }
        }
        m_rows.clear();
    }
    
    int FCDataTable::columnsCount(){
        return m_columns.size();
    }
    
    int FCDataTable::getColumnIndex(int colName){
        if(colName == FCDataTable::NULLFIELD()){
            return -1;
        }
        int low = 0;
        int high = m_columns.size() - 1;
        while (low <= high) {
            int middle = (low + high) / 2;
            int *hf = m_columns.get(middle);
            if (colName == hf[0]) {
                return middle;
            }
            else if (colName > hf[0]) {
                low = middle + 1;
            }
            else if (colName < hf[0]) {
                high = middle - 1;
            }
        }
        return -1;
    }
    
    int FCDataTable::getRowIndex(double key){
        int low = 0;
        int high = m_keys.size() - 1;
        while (low <= high) {
            int middle = (low + high) / 2;
            double hf = m_keys.get(middle);
            if (key == hf) {
                return middle;
            }
            else if (key > hf) {
                low = middle + 1;
            }
            else if (key < hf) {
                high = middle - 1;
            }
        }
        return -1;
    }
    
    double FCDataTable::getXValue(int rowIndex){
        return m_keys.get(rowIndex);
    }
    
    double FCDataTable::get(double pk, int colName){
        int index = FCDataTable::getRowIndex(pk);
        if(index >= 0 && index < m_rows.size()){
            int colIndex = getColumnIndex(colName);
            return m_rows.get(index)->get(colIndex);
        }
        return NaN;
    }
    
    double FCDataTable::get2(int rowIndex, int colName){
        if(rowIndex >= 0 && rowIndex < m_rows.size()){
            int colIndex = getColumnIndex(colName);
            return m_rows.get(rowIndex)->get(colIndex);
        }
        return NaN;
    }
    
    double FCDataTable::get3(int rowIndex, int colIndex){
        if(rowIndex >= 0 && rowIndex < m_rows.size()){
            return m_rows.get(rowIndex)->get(colIndex);
        }
        return NaN;
    }
    
    void FCDataTable::removeAt(int rowIndex){
        m_keys.removeAt(rowIndex);
        FCDataRow *row = m_rows.get(rowIndex);
        delete row;
        m_rows.removeAt(rowIndex);
    }
    
    void FCDataTable::removeColumn(int colName){
        int colIndex = getColumnIndex(colName);
        if(colIndex != -1){
            int removeIndex = -1;
            int *removeColumn = 0;
            for (int i = 0; i < m_columns.size(); i++){
                int *col = m_columns.get(i);
                int name = col[0];
                int index = col[1];
                if (col[0] == colName){
                    removeIndex = i;
                    removeColumn = col;
                }
                else{
                    if (index > colIndex){
                        int *newcol = new int[2];
                        newcol[0] = name;
                        newcol[1] = index - 1;
                        m_columns.set(i, newcol);
                        delete[] col;
                        col = 0;
                    }
                }
            }
            if(removeColumn){
                delete[] removeColumn;
            }
            m_columns.removeAt(removeIndex);
            for (int i = 0; i < m_rows.size(); i++){
                m_rows.get(i)->remove(colIndex);
                m_rows.get(i)->fillEmpty(m_columns.size());
            }
        }
    }
    
    int FCDataTable::rowsCount(){
        return m_keys.size();
    }
    
    void FCDataTable::set(double pk, int colName, double value){
        FCDataRow *row = 0;
        int index = FCDataTable::getRowIndex(pk);
        if(index == -1){
            row = addKey(pk);
            row->fillEmpty(m_columns.size());
        }
        else{
            row = m_rows.get(index);
        }
        int colIndex = getColumnIndex(colName);
        row->set(colIndex,value);
    }
    
    void FCDataTable::set2(int rowIndex, int colName, double value){
        int colIndex = getColumnIndex(colName);
        m_rows.get(rowIndex)->set(colIndex, value);
    }
    
    void FCDataTable::set3(int rowIndex, int colIndex, double value){
        m_rows.get(rowIndex)->set(colIndex, value);
    }
    
    void FCDataTable::setColsCapacity(int capacity){
        m_colsCapacity = capacity;
    }
    
    void FCDataTable::setColsGrowStep(int step){
        m_colsStep = step;
    }
    
    void FCDataTable::setRowsCapacity(int capacity){
        m_keys.setCapacity(capacity);
        m_rows.setCapacity(capacity);
    }
    
    void FCDataTable::setRowsGrowStep(int step){
        m_keys.setStep(step);
        m_rows.setStep(step);
    }
    
    double* FCDataTable::DATA_ARRAY(int colName, int rowIndex, int n, int *length){
        if (rowIndex >= 0){
            int arraylength = n;
            int start = 0;
            if (rowIndex < n - 1){
                arraylength = rowIndex + 1;
            }
            else{
                start = rowIndex - n + 1;
            }
            if (arraylength == -1){
                *length = 0;
                return new double[0];
            }
            double *ary = new double[arraylength];
            int colIndex = getColumnIndex(colName);
            for (int i = start; i <= rowIndex; i++){
                ary[i - start] = get3(i, colIndex);
            }
            *length = arraylength;
            return ary;
        }
        else{
            *length = 0;
            return new double[0];
        }
    }
}
