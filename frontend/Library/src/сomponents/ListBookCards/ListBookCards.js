import React, { useEffect, useState } from 'react';
import BookCard from '../BookCard/BookCard'; // Импортируем компонент карточки книги
import { Row, Col, Pagination, Spin } from 'antd'; // Используем Ant Design для сетки и пагинации

const BookList = () => {
  const [books, setBooks] = useState([]);
  const [genres, setGenres] = useState([]);
  const [authors, setAuthors] = useState([]);
  const [loading, setLoading] = useState(false); // Состояние для отслеживания загрузки
  const [page, setPage] = useState(1); // Текущая страница
  const [pageSize, setPageSize] = useState(5); // Количество элементов на странице
  const [totalBooks, setTotalBooks] = useState(0); // Общее количество книг

  // Функция для получения жанров
  const fetchGenres = async () => {
    try {
      const response = await fetch('https://localhost:7040/Genre', {
        method: 'GET',
        credentials: 'include', // Для отправки куков на сервер
      });
      const data = await response.json();
      setGenres(data);
      return data;
    } catch (error) {
      console.error('Error fetching genres:', error);
      return [];
    }
  };

  // Функция для получения авторов
  const fetchAuthors = async () => {
    try {
      const response = await fetch('https://localhost:7040/Author', {
        method: 'GET',
        credentials: 'include',
      });
      const data = await response.json();
      setAuthors(data);
      return data;
    } catch (error) {
      console.error('Error fetching authors:', error);
      return [];
    }
  };

  const fetchAllBook = async () =>{
    const response = await fetch('https://localhost:7040/Books/count', {
      method: 'GET',
      credentials: 'include',
    });
    var data = await response.json()
    setTotalBooks(data);
    console.log(totalBooks);
  }

  // Функция для получения книг с поддержкой пагинации
  const fetchBooks = async (page, pageSize) => {
    setLoading(true); // Включаем индикатор загрузки
    try {
      // Корректный URL для запроса с параметрами пагинации
      const response = await fetch(`https://localhost:7040/Books/getBypage/${page}/${pageSize}`, {
        method: 'GET',
        credentials: 'include'
      });

      const data = await response.json();
      const genresData = await fetchGenres();
      const authorsData = await fetchAuthors();
      await fetchAllBook();

      // Добавляем жанры и авторов к книгам
      const booksWithGenresAndAuthors = data.map((book) => {
        const genre = genresData.find((g) => g.id === book.genreId);
        const author = authorsData.find((a) => a.id === book.authorId);

        return {
          ...book,
          genreName: genre ? genre.genre : 'Unknown Genre',
          authorName: author ? author.name + " " + author.surname : 'Unknown Author',
        };
      });

      setBooks(booksWithGenresAndAuthors);
    } catch (error) {
      console.error('Error fetching books:', error);
    } finally {
      setLoading(false); // Выключаем индикатор загрузки
    }
  };

  useEffect(() => {
    fetchBooks(page, pageSize); // Загружаем книги при изменении страницы
  }, [page, pageSize]);

  // Обработчик изменения страницы
  const handlePageChange = (page, pageSize) => {
    setPage(page);
    setPageSize(pageSize);
  };

  return (
    <div >

      {loading ? (
        <Spin tip="Loading books..." />
      ) : (
        <>
        
        <Row
            gutter={[16, 32]} // Отступы между колонками
            style={{ display: 'flex', justifyContent: 'center', alignItems: "center" }} 
          >
            {books.map((book) => (
              <Col
              key={book.id}
              span={19} // ширинa колонок 
              style={{ display: 'flex', justifyContent: 'center', alignItems: "center" }} // Центрируем карточки в колонках
            >
                <BookCard
                  title={book.title}
                  author={book.authorName}
                  genre={book.genreName}
                  returnDate={book.returnDate}
                  coverImagePath={book.coverImagePath}
                />
              </Col>
            ))}
          </Row>

          <div style={{ display: 'flex', justifyContent: 'center', marginTop: '20px' }}>
          <Pagination
            current={page}
            pageSize={pageSize}
            total={totalBooks}
            onChange={handlePageChange}
            showSizeChanger
            pageSizeOptions={[5, 10, 20, 50]}
          />
          </div>
        </>
      )}
    </div>
  );
};

export default BookList;
