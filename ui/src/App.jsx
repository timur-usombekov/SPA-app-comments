import React from 'react'
import '@blueprintjs/core/lib/css/blueprint.css';
import CommentForm from './CommentForm';
import CommentList from './CommentList';
import './App.css';

function App() {
	return (
		<div className="centered-container">
			<CommentForm />
			<h3>Comments:</h3>
			<CommentList/>
		</div >
	);
}

export default App; 
