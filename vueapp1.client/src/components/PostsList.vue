<template>
  <div class="posts-container">
    <h2>Posts</h2>

    <!-- Form for creating a new post -->
    <form @submit.prevent="createPost">
      <input v-model="newPost.title" placeholder="Title" required />
      <textarea v-model="newPost.body" placeholder="Body" required></textarea>
      <button type="submit">Create Post</button>
    </form>

    <!-- Display loading message while posts are being fetched -->
    <div v-if="loading">Loading posts...</div>

    <!-- Show posts list -->
    <ul v-else style="list-style-type: none; padding-left: 0;">
      <li v-for="post in posts" :key="post.id" style="padding: 10px 0; border-bottom: 1px solid #ddd;">
        <div style="display: flex; justify-content: space-between; align-items: center;">
          <span style="font-weight: bold">{{ post.title }}</span>

          <!-- Buttons for each post aligned to the right -->
          <div>
            <button @click="editPost(post)" style="background-color: #4CAF50; color: white; padding: 8px 16px; border: none; border-radius: 4px;">Edit</button>
            <button @click="patchPost(post)" style="background-color: #FF9800; color: white; padding: 8px 16px; border: none; border-radius: 4px;">Patch</button>
            <button @click="deletePost(post.id)" style="background-color: #f44336; color: white; padding: 8px 16px; border: none; border-radius: 4px;">Delete</button>
          </div>
        </div>

        <!-- Post body -->
        <p>{{ post.body }}</p>
      </li>
    </ul>

    <!-- Display any error message -->
    <div v-if="error" style="color: red">{{ error }}</div>

    <!-- Edit post modal -->
    <div v-if="isEditing" class="modal">
      <div class="modal-content">
        <h3>Edit Post</h3>
        <input v-model="editingPost.title" placeholder="Title" required />
        <textarea v-model="editingPost.body" placeholder="Body" required></textarea>
        <button @click="updatePost(editingPost.id)">Save</button>
        <button @click="closeEditModal">Cancel</button>
      </div>
    </div>

    <!-- Patch post modal -->
    <div v-if="isPatching" class="modal">
      <div class="modal-content">
        <h3>Patch Post</h3>
        <input v-model="patchData.title" placeholder="New Title" />
        <button @click="applyPatch">Patch Title</button>
        <button @click="closePatchModal">Cancel</button>
      </div>
    </div>
  </div>
</template>

<script setup>
  import { ref, onMounted } from 'vue'

  const posts = ref([])
  const newPost = ref({ title: '', body: '' })
  const loading = ref(false)
  const error = ref(null)
  const isEditing = ref(false)
  const isPatching = ref(false)
  const editingPost = ref({ id: null, title: '', body: '' })
  const patchData = ref({ id: null, title: '' })

  const fetchPosts = async () => {
    loading.value = true
    try {
      const res = await fetch('/api/posts')
      if (!res.ok) throw new Error('Failed to fetch posts. Try refreshing the page.')
      posts.value = await res.json()
    } catch (err) {
      error.value = err.message
    } finally {
      loading.value = false
    }
  }

  const createPost = async () => {
    try {
      const res = await fetch('/api/posts', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(newPost.value)
      })
      if (!res.ok) throw new Error('Failed to create post')
      const post = await res.json()
      posts.value.push(post)
      newPost.value = { title: '', body: '' }
    } catch (err) {
      error.value = err.message
    }
  }

  const deletePost = async (id) => {
    try {
      const res = await fetch(`/api/posts/${id}`, { method: 'DELETE' })
      if (!res.ok) throw new Error('Failed to delete post')
      posts.value = posts.value.filter(p => p.id !== id)
    } catch (err) {
      error.value = err.message
    }
  }

  const editPost = (post) => {
    editingPost.value = { ...post }
    isEditing.value = true
  }

  const closeEditModal = () => {
    isEditing.value = false
    editingPost.value = { id: null, title: '', body: '' }
  }

  const updatePost = async (id) => {
    try {
      const res = await fetch(`/api/posts/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(editingPost.value)
      })
      if (!res.ok) throw new Error('Failed to update post. Self-created posts cannot be updated due to JSONPlaceholder restrictions.')
      const updatedPost = await res.json()
      const index = posts.value.findIndex(p => p.id === id)
      posts.value[index] = updatedPost
      closeEditModal()
    } catch (err) {
      error.value = err.message
    }
  }

  const patchPost = (post) => {
    patchData.value = { id: post.id, title: post.title }
    isPatching.value = true
  }

  const closePatchModal = () => {
    isPatching.value = false
    patchData.value = { id: null, title: '' }
  }

  const applyPatch = async () => {
    try {
      if (!patchData.value || !patchData.value.id) {
        throw new Error("Post ID is missing in the patch data.")
      }

      const patchPayload = {
        id: patchData.value.id,
        title: patchData.value.title,
        body: posts.value.find(p => p.id === patchData.value.id)?.body || ''
      }

      const res = await fetch(`/api/posts/${patchData.value.id}`, {
        method: 'PATCH',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(patchPayload),
      })

      if (!res.ok) throw new Error('Failed to patch post. Self-created posts cannot be patched due to JSONPlaceholder restrictions.')

      const patchedPost = await res.json()
      const index = posts.value.findIndex(p => p.id === patchedPost.id)
      if (index !== -1) posts.value[index] = patchedPost
      isPatching.value = false
    } catch (err) {
      error.value = err.message
    }
  }

  onMounted(fetchPosts)
</script>

<style scoped>
  .posts-container {
    max-width: 800px; /* Limiting the maximum width */
    margin: 0 auto; /* Centering the container */
    padding: 16px;
    box-sizing: border-box;
  }

  input, textarea {
    width: 100%;
    margin: 10px 0;
    padding: 10px;
    border-radius: 4px;
    border: 1px solid #ddd;
  }

  button {
    background-color: #007BFF;
    color: white;
    border: none;
    padding: 10px 15px;
    border-radius: 4px;
    cursor: pointer;
    margin-top: 10px;
  }

    button:hover {
      background-color: #0056b3;
    }

  .modal {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
    background: rgba(0, 0, 0, 0.5);
  }

  .modal-content {
    background: white;
    padding: 20px;
    border-radius: 8px;
    max-width: 400px;
    width: 100%;
  }
</style>

