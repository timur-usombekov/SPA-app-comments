import React, { useEffect, useState } from 'react';
import axios from 'axios';
import CommentCard from './CommentCard'

const CommentList = () => {
    const [comments, setComments] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchComments = async () => {
            try {
                const response = await axios.get('https://localhost:7137/comment');
                setComments(response.data);
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        fetchComments();
    }, []);

    if (loading) {
        return <p>Loading comments...</p>;
    }

    if (error) {
        return <p>Error loading comments: {error}</p>;
    }

    return (
        <div>
            {comments.map(comment => (
                <CommentCard
                    key={comment.id}
                    username={comment.user.name}
                    email={comment.user.email}
                    text={comment.text}
                />
            ))}
        </div>
    );
};

export default CommentList;
