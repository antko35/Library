import {React, useState} from 'react'
import BookList from '../../сomponents/ListBookCards/ListBookCards'
import {  Input } from 'antd';
const AllBooks = () => {
  const { Search } = Input;
  const [search, setSearch] = useState('');

  const handleSearchChange = (e) => {
    const value = e.target.value;
    setSearch(value); // Обновляем вводимый текст
   
  };

  return (
    <div style={{ padding: '20px' , textAlign: 'center'  }}>
       <Search
          placeholder="Введите название книги или автора"
          allowClear
          value={search}
          onChange={handleSearchChange}
          style={{ width: '300px'}}
        />
    <BookList search={search} />

  </div>
  )
}

export default AllBooks