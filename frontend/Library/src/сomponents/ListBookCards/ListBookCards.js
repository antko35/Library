import React, { useEffect, useState } from 'react';
import BookCard from '../BookCard/BookCard'; // Импортируем компонент карточки книги
import { Row, Col, Pagination, Spin, Empty,Select, Button,Flex } from 'antd'; // Добавляем компонент Empty для пустого состояния
import { useAuth } from '../../context/AuthContext';
import AddAuthor from '../AddAuthor/AddAuthor';
import AddGenre from '../AddGenre/AddGenre';
import AddBook from '../AddBook/AddBook';
import AdminTools from '../AdminTools/AdminTools';
const { Option } = Select;

const BookList = () => {
  const [books, setBooks] = useState([]);
  const [genres, setGenres] = useState([]);
  const [authors, setAuthors] = useState([]);
  const [loading, setLoading] = useState(false); // Состояние для отслеживания загрузки
  const [page, setPage] = useState(1); // Текущая страница
  const [pageSize, setPageSize] = useState(5); // Количество элементов на странице
  const [totalBooks, setTotalBooks] = useState(0); // Общее количество книг

  const [selectedGenre, setSelectedGenre] = useState(null);
  const [selectedAuthor, setSelectedAuthor] = useState(null);

  const {user} = useAuth();
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

  const fetchAllBook = async () => {
    const response = await fetch('https://localhost:7040/Books/count', {
      method: 'GET',
      credentials: 'include',
    });
    const data = await response.json();
    setTotalBooks(data);
  };

  // Функция для получения книг с поддержкой пагинации
  const fetchBooks = async (page, pageSize) => {
    setLoading(true); // Включаем индикатор загрузки
    try {
      // Корректный URL для запроса с параметрами пагинации
      const response = await fetch(`https://localhost:7040/Books/getBypage/${page}/${pageSize}`, {
        method: 'GET',
        credentials: 'include',
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
          authorName: author ? author.name + ' ' + author.surname : 'Unknown Author',
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


  const handleBorrow = async (bookId) => {
    try {
      const response = await fetch(`https://localhost:7040/Books/borrow/${bookId}`, {
        method: 'POST',
        credentials: 'include', 
      });
      if (response.ok) {
        fetchBooks(page, pageSize);
        console.log('Book successfully borrowed!');

      } else {
        console.error('Error borrowing the book.');
      }
    } catch (error) {
      console.error('Failed to borrow the book.', error);
    }
  };

  const handleCreateBook = async (values) =>{
    try {
      const response = await fetch('https://localhost:7040/Books/create', {
        method: 'POST',
        credentials: 'include',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(values),
      });
      if (response.ok) {
        fetchBooks(page, pageSize);
        console.log('Book successfully created!');
        return response;
        setIsModalBookVisible(false);
      } else {
        console.error('Error creating the book.');
      }
    } catch (error) {
      console.error('Failed to create the book.', error);
    }
  }
  const [isModalVisible, setIsModalVisible] = useState(false); // for author
  const [isModalGenreVisible, setIsModalGenreVisible] = useState(false);
  const [isModalBookVisible, setIsModalBookVisible] = useState(false);

  return (
    <div>
      {loading ? (
        <Spin tip="Loading books..." />
      ) : (
        <>
          {books.length === 0 ? (
            <Empty description="No books found" />
          ) : (
            <>
            {user.isAdmin && (
              <AdminTools setIsModalVisible={setIsModalVisible} setIsModalGenreVisible={setIsModalGenreVisible} setIsModalBookVisible={setIsModalBookVisible} />
            )}
              <Row
                gutter={[16, 32]} // Отступы между колонками
                style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}
              >
                
                {books.map((book) => (
                  <Col
                    key={book.id}
                    span={20}
                    style={{
                      display: 'flex',
                      justifyContent: 'center',
                      alignItems: 'center',
                    }}
                  >
                    <BookCard
                      id = {book.id}
                      title={book.title}
                      author={book.authorName}
                      genre={book.genreName}
                      inProfile={book.inProfile}
                      coverImagePath={book.coverImagePath}
                      onBorrow={() => handleBorrow(book.id)}
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
              <AddAuthor visible={isModalVisible} onClose={() => setIsModalVisible(false)} />
              <AddGenre visible={isModalGenreVisible} onClose={() => setIsModalGenreVisible(false)} />
              <AddBook visible={isModalBookVisible} onCreate ={handleCreateBook} onClose={() => setIsModalBookVisible(false)} />
            </>
          )}
        </>
      )}
    </div>
  );
};

export default BookList;
