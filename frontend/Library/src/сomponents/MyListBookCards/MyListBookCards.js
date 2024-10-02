import React, { useEffect, useState } from 'react';
import MyBookCard from '../MyBookCard/MyBookCard'; 
import { Row, Col, Spin, Empty } from 'antd'; 

const MyBookList = () => {
  const [books, setBooks] = useState([]);
  const [genres, setGenres] = useState([]);
  const [authors, setAuthors] = useState([]);
  const [loading, setLoading] = useState(false); 

  const fetchGenres = async () => {
    try {
      const response = await fetch('https://localhost:7040/Genre', {
        method: 'GET',
        credentials: 'include', 
      });
      const data = await response.json();
      setGenres(data);
      return data;
    } catch (error) {
      console.error('Error fetching genres:', error);
      return [];
    }
  };

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
    const response = await fetch('https://localhost:7040/User/info', {
      method: 'GET',
      credentials: 'include',
    });
    var data = await response.json();
    return data;
  }

  const fetchBooks = async () => {
    setLoading(true);
    try {
      const genresData = await fetchGenres();
      const authorsData = await fetchAuthors();
      const userInfo = await fetchAllBook();

      const booksWithGenresAndAuthors = userInfo.books.map((book) => {
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
      setLoading(false); 
    }
  };

  const handleReturn = async (bookId) => {
    try {
      const response = await fetch(`https://localhost:7040/Books/return/${bookId}`, {
        method: 'POST',
        credentials: 'include', 
      });
      if (response.ok) {
        fetchBooks();
        console.success('Book successfully borrowed!');

      } else {
        console.error('Error borrowing the book.');
      }
    } catch (error) {
      console.error('Failed to borrow the book.');
    }
  };

  useEffect(() => {
    fetchBooks();
  }, []);

  return (
    <div>
      {loading ? (
        <Spin tip="Loading books..." />
      ) : (
        <>
          {books.length === 0 ? ( 
            <Empty description="No books found" />
          ) : (
            <Row
              gutter={[16, 32]} 
              style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }} 
            >
              {books.map((book) => (
                <Col
                  key={book.id}
                  span={20} 
                  style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}
                >
                  <MyBookCard
                    title={book.title}
                    author={book.authorName}
                    genre={book.genreName}
                    returnDate={book.returnDate}
                    coverImagePath={book.coverImagePath}
                    onReturn={() => handleReturn(book.id)}
                  />
                </Col>
              ))}
            </Row>
          )}
        </>
      )}
    </div>
  );
};

export default MyBookList;
