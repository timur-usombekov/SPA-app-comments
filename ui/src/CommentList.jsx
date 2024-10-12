import React, { useEffect, useState } from 'react';
import axios from 'axios';
import CommentCard from './CommentCard';
import { Spinner, Callout, HTMLSelect, Button } from '@blueprintjs/core';

const CommentList = () => {
    const [comments, setComments] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [sortBy, setSortBy] = useState('');
    const [currentPage, setCurrentPage] = useState(1);
    const commentsPerPage = 25;
    
    const fetchComments = async () => {
        setLoading(true);
        try {
            const url = `http://localhost:8001/comment?sortBy=${sortBy}`;
            const response = await axios.get(url);
            
            const commentsWithFiles = response.data.map(comment => {
                if (comment.file) {
                    const byteCharacters = atob(comment.file);
                    const byteNumbers = new Uint8Array(byteCharacters.length);
                    for (let i = 0; i < byteCharacters.length; i++) {
                        byteNumbers[i] = byteCharacters.charCodeAt(i);
                    }
                    const blob = new Blob([byteNumbers], { type: 'application/octet-stream' });
                    const file = new File([blob], 'attachment');
                    return { ...comment, file };
                }
                return comment;
            });

            setComments(commentsWithFiles);
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchComments();
    }, []); 

    const handleSortChange = (event) => {
        setSortBy(event.target.value);
        setCurrentPage(1); 
    };

    const sortedComments = [...comments].sort((a, b) => {
        if (sortBy === 'date') {
            return new Date(a.createdAt) - new Date(b.createdAt);
        } else if (sortBy === 'username') {
            return a.user.name.localeCompare(b.user.name);
        } else if (sortBy === 'email') {
            return a.user.email.localeCompare(b.user.email);
        }
        return 0; 
    });

    const indexOfLastComment = currentPage * commentsPerPage;
    const indexOfFirstComment = indexOfLastComment - commentsPerPage;
    const currentComments = sortedComments.slice(indexOfFirstComment, indexOfLastComment);
    const totalComments = sortedComments.length;

    if (loading) {
        return (
            <div style={{ textAlign: 'center' }}>
                <Spinner />
                <p>Loading comments...</p>
            </div>
        );
    }

    if (error) {
        return (
            <Callout intent="danger" title="Error">
                Error loading comments: {error}
            </Callout>
        );
    }

    return (
        <div>
            <div style={{ marginBottom: '15px' }}>
                <HTMLSelect onChange={handleSortChange} value={sortBy}>
                    <option value="">Sort by Date(descending)</option>
                    <option value="date">Sort by Date(ascending)</option>
                    <option value="username">Sort by Username</option>
                    <option value="email">Sort by Email</option>
                </HTMLSelect>
            </div>

            {currentComments.length === 0 ? (
                <p>No comments found.</p>
            ) : (
                <>
                    {currentComments.map(comment => (
                        <CommentCard
                            key={comment.id}
                            commentId={comment.id}
                            username={comment.user.name}
                            email={comment.user.email}
                            text={comment.text}
                            createdAt={comment.createdAt}
                            file={comment.file}
                            fileExtension={comment.fileExtension}
                            url={comment.url}
                        />
                    ))}

                    <div style={{ marginTop: '15px', textAlign: 'center' }}>
                        <Button 
                            disabled={currentPage === 1} 
                            onClick={() => setCurrentPage(prev => Math.max(prev - 1, 1))}
                        >
                            Previous
                        </Button>
                        <span style={{ margin: '0 10px' }}>Page {currentPage} of {Math.ceil(totalComments / commentsPerPage)}</span>
                        <Button 
                            disabled={currentPage * commentsPerPage >= totalComments} 
                            onClick={() => setCurrentPage(prev => prev + 1)}
                        >
                            Next
                        </Button>
                    </div>
                </>
            )}
        </div>
    );
};

export default CommentList;
