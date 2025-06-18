export default function Home() {
  return (
    <div className="p-6">
      <h1 className="text-black dark:text-white text-3xl font-bold mb-6">Welcome</h1>
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-sm border border-gray-700 dark:border-gray-200 ">
          <h2 className="text-xl text-black dark:text-white font-semibold mb-3">Card 1</h2>
          <p className="text-gray-600 dark:text-gray-300">Sample content for your dashboard.</p>
        </div>
        <div className="bg-white text-black dark:text-white dark:bg-gray-800 p-6 rounded-lg shadow-sm border border-gray-700 dark:border-gray-200">
          <h2 className="text-xl font-semibold mb-3">Card 2</h2>
          <p className="text-gray-600 dark:text-gray-300">More content to fill your layout.</p>
        </div>
        <div className="bg-white text-black dark:text-white dark:bg-gray-800 p-6 rounded-lg shadow-sm border border-gray-700 dark:border-gray-200">
          <h2 className="text-xl font-semibold mb-3">Card 3</h2>
          <p className="text-gray-600 dark:text-gray-300">Additional content for the grid.</p>
        </div>
      </div>
    </div>
  );
}
